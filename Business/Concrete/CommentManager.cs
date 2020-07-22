﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Business.Abstract;
using Business.ValidationRules.FluentValidation;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;

namespace Business.Concrete
{
    public class CommentManager : ICommentService
    {
        private ICommentDal _commentDal;

        private Validation<CommentValidator> _validation;
        public CommentManager(ICommentDal commentDal)
        {
            _commentDal = commentDal;
        }

        public IDataResult<List<Photo>> GetPhotosByUserComment(int userId)
        {
            return new SuccessDataResult<List<Photo>>(_commentDal.GetPhotosByUserComment(new User { Id = userId }));
        }

        public IDataResult<List<User>> GetUsersByPhotoComment(int photoId)
        {
            return new SuccessDataResult<List<User>>(_commentDal.GetUsersByPhotoComment(new Photo { Id = photoId }));
        }

        public IDataResult<List<Comment>> GetPhotoComments(int photoId)
        {
            return new SuccessDataResult<List<Comment>>(_commentDal.GetPhotoComments(new Photo { Id = photoId }).ToList());
        }

        public IResult Delete(Comment comment)
        {
            _commentDal.Delete(comment);
            return new SuccessDataResult<Comment>(comment);
        }

        public IResult Add(Comment comment)
        {
            _validation = new Validation<CommentValidator>();
            _validation.Validate(comment);
            _commentDal.Add(comment);
            return new SuccessDataResult<Comment>(comment);
        }

        public IResult Update(Comment comment)
        {
            _validation = new Validation<CommentValidator>();
            _validation.Validate(comment);
            _commentDal.Update(comment);
            return new SuccessResult();
        }
    }
}
