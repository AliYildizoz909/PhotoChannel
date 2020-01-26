﻿using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IUserService
    {
        IDataResult<User> GetById(int id);
        IDataResult<UserDetail> GetUserDetailById(int id);
        IDataResult<List<User>> GetList();
        IDataResult<User> GetByEmail(string email);
        IDataResult<List<Photo>> GetPhotos(User user);

        IDataResult<List<OperationClaim>> GetClaims(User user);
        IDataResult<List<Channel>> GetSubscriptions(User user);
        IDataResult<List<Photo>> GetLikedPhotos(User user);
        IDataResult<User> Delete(User user);
        IDataResult<User> Add(User user);
        IDataResult<User> Update(User user);

    }
}
