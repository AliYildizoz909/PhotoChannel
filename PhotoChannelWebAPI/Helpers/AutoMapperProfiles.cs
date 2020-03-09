﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Entities.Concrete;
using Core.Utilities.Security.Jwt;
using Entities.Concrete;
using Entities.Dtos;
using PhotoChannelWebAPI.Dtos;

namespace PhotoChannelWebAPI.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<ChannelForAddDto, Channel>();
            CreateMap<ChannelForUpdateDto, Channel>();
            CreateMap<Channel, ChannelForListDto>();
            CreateMap<User, SubscriberForListDto>();
            CreateMap<User, ChannelForAdminListDto>();
            CreateMap<ChannelForDetailDto, Channel>();

            CreateMap<Photo, PhotoForListDto>();
            CreateMap<PhotoForAddDto, Photo>();
            CreateMap<Photo, PhotoForDetailDto>();
            CreateMap<User, LikeForUserListDto>();

            CreateMap<CommentForAddDto, Comment>();
            CreateMap<CommentForUpdateDto, Comment>();
            CreateMap<Comment, CommentForListDto>();

            CreateMap<User, UserForListDto>();
            CreateMap<User, UserForDetailDto>();
            CreateMap<User, CurrentUserDto>();
            CreateMap<UserForUpdateDto, User>();
        }
    }
}
