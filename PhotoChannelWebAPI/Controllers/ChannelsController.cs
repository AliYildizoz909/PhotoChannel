﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Abstract;
using Castle.Core.Internal;
using CloudinaryDotNet.Actions;
using Core.Entities.Concrete;
using Core.Utilities.PhotoUpload;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.Dtos;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PhotoChannelWebAPI.Dtos;
using PhotoChannelWebAPI.Helpers;

namespace PhotoChannelWebAPI.Controllers
{
    [Route("api/channels")]
    [ApiController]
    public class ChannelsController : ControllerBase
    {
        private IChannelService _channelService;
        private IMapper _mapper;
        private IPhotoUpload _photoUpload;
        private IAuthHelper _authHelper;
        private ICountService _countService;
        public ChannelsController(IChannelService channelService, IMapper mapper, IPhotoUpload photoUpload, IAuthHelper authHelper, ICountService countService)
        {
            _channelService = channelService;
            _mapper = mapper;
            _photoUpload = photoUpload;
            _authHelper = authHelper;
            _countService = countService;
        }

        [HttpPost]
        public IActionResult Post([FromForm]ChannelForAddDto channelForAddDto)
        {
            string ownerId = _authHelper.GetCurrentUserId();
            if (!string.IsNullOrEmpty(ownerId))
            {
                IResult checkResult = _channelService.CheckIfChannelNameExists(channelForAddDto.Name);
                if (!checkResult.IsSuccessful)
                {
                    return BadRequest(checkResult.Message);
                }

                if (channelForAddDto.File.Length > 0)
                {
                    ImageUploadResult imageUploadResult = _photoUpload.ImageUpload(channelForAddDto.File);
                    var channel = _mapper.Map<Channel>(channelForAddDto);
                    channel.ChannelPhotoUrl = imageUploadResult.Uri.ToString();
                    channel.PublicId = imageUploadResult.PublicId;
                    //channel.UserId = int.Parse(ownerId);
                    IResult result = _channelService.Add(channel);
                    if (result.IsSuccessful)
                    {
                        var mapResult = _mapper.Map<ChannelForDetailDto>(channel);
                        return Ok(mapResult);
                    }

                    return BadRequest(result.Message);
                }
            }
            return BadRequest();
        }

        [HttpDelete]
        [Route("{channelId}")]
        public IActionResult Delete(int channelId)
        {
            if (channelId > 0)
            {
                IResult result = _channelService.Delete(channelId);
                if (result.IsSuccessful)
                {
                    return Ok(result.Message);
                }
                return BadRequest(result.Message);
            }


            return BadRequest();
        }


        [HttpPut]
        [Route("{channelId}")]
        public IActionResult Put(int channelId, [FromForm]ChannelForUpdateDto channelForUpdate)
        {
            var dataResult = _channelService.GetById(channelId);
            if (dataResult.IsSuccessful)
            {
                if (channelForUpdate.File != null || !channelForUpdate.Name.IsNullOrEmpty())
                {
                    if (!channelForUpdate.Name.IsNullOrEmpty())
                    {
                        IResult result = _channelService.CheckIfChannelNameExistsWithUpdate(channelForUpdate.Name, channelId);
                        if (result.IsSuccessful)
                        {
                            return BadRequest(result.Message);
                        }

                        dataResult.Data.Name = channelForUpdate.Name;
                    }
                    if (channelForUpdate.File != null && channelForUpdate.File.Length > 0)
                    {
                        DeletionResult deletionResult = _photoUpload.ImageDelete(dataResult.Data.PublicId);
                        ImageUploadResult imageUploadResult = _photoUpload.ImageUpload(channelForUpdate.File);
                        dataResult.Data.ChannelPhotoUrl = imageUploadResult.Uri.ToString();
                        dataResult.Data.PublicId = imageUploadResult.PublicId;
                    }
                    IDataResult<Channel> updateDataResult = _channelService.Update(dataResult.Data, channelId);
                    if (dataResult.IsSuccessful)
                    {
                        var map = _mapper.Map<ChannelForDetailDto>(updateDataResult.Data);
                        return Ok(map);
                    }
                }
                return BadRequest();
            }
            return NotFound(dataResult.Message);
        }


        [HttpGet]
        public IActionResult Get()
        {
            IDataResult<List<Channel>> result = _channelService.GetList();
            if (result.IsSuccessful)
            {
                var mapResult = _mapper.Map<List<ChannelForListDto>>(result.Data);
                return Ok(mapResult);
            }

            return BadRequest(result.Message);
        }
        [HttpGet]
        public IActionResult GetChannelByName(string channelName)
        {
            if (string.IsNullOrEmpty(channelName))
            {
                IDataResult<List<Channel>> result = _channelService.GetByName(channelName);
                if (result.IsSuccessful)
                {
                    var mapResult = _mapper.Map<List<ChannelForListDto>>(result.Data);
                    return Ok(mapResult);
                }
                return NotFound(result.Message);
            }
            return BadRequest();
        }
        [HttpGet]
        [Route("{channelId}")]
        public IActionResult GetChannelById(int channelId)
        {
            if (channelId > 0)
            {
                IDataResult<Channel> result = _channelService.GetById(channelId);
                if (result.IsSuccessful)
                {
                    var mapResult = _mapper.Map<ChannelForDetailDto>(result.Data);
                    mapResult.SubscribersCount = _countService.GetSubscriberCount(channelId).Data;
                    return Ok(mapResult);
                }

                return NotFound(result.Message);
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("{channelId}/owner")]
        public IActionResult GetOwner(int channelId)
        {
            if (channelId > 0)
            {
                IDataResult<User> result = _channelService.GetOwner(channelId);
                if (result.IsSuccessful)
                {
                    var mapResult = _mapper.Map<UserForDetailDto>(result.Data);
                    return Ok(mapResult);
                }
                return NotFound(result.Message);
            }
            return BadRequest();
        }
    }
}