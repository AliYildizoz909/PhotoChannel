﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Business.Abstract;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Validation;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;

namespace Business.Concrete
{
    public class PhotoManager : IPhotoService
    {
        private IPhotoDal _photoDal;

        public PhotoManager(IPhotoDal photoDal)
        {
            _photoDal = photoDal;
        }


        public IDataResult<List<Photo>> GetChannelPhotos(int channelId)
        {
            return new SuccessDataResult<List<Photo>>(_photoDal.GetChannelPhotos(new Channel { Id = channelId }));
        }

        public IDataResult<List<Photo>> GetUserPhotos(int userId)
        {
            return new SuccessDataResult<List<Photo>>(_photoDal.GetUserPhotos(new User { Id = userId }));
        }

        [ValidationAspect(typeof(PhotoValidator), Priority = 1)]
        [CacheAspect]
        public IDataResult<Photo> GetById(int id)
        {
            var photo = _photoDal.Get(p => p.Id == id);
            if (photo != null)
            {
                return new SuccessDataResult<Photo>(photo);
            }
            return new ErrorDataResult<Photo>(Messages.PhotoNotFound);
        }

        [CacheRemoveAspect("IPhotoService.Get")]
        public IResult Delete(Photo photo)
        {
            _photoDal.Delete(photo);
            return new SuccessResult();
        }

        [ValidationAspect(typeof(PhotoValidator), Priority = 1)]
        [CacheRemoveAspect("IPhotoService.Get")]
        public IDataResult<Photo> Add(Photo photo)
        {
            _photoDal.Add(photo);
            return new SuccessDataResult<Photo>(photo);
        }
        public IResult Exists(int id)
        {
            var photo = _photoDal.Get(p => p.Id == id);
            if (photo != null)
            {
                return new SuccessResult();
            }
            return new ErrorResult(Messages.PhotoNotFound);
        }
    }
}
