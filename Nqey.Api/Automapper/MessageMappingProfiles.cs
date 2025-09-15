using AutoMapper;
using Nqey.Api.Dtos.MessageDtos;
using Nqey.Domain;

namespace Nqey.Api.Automapper
{
    public class MessageMappingProfiles: Profile
    {

        public MessageMappingProfiles() {

            CreateMap<MessagePostPutDto, Message>();
            CreateMap<Message, MessageGetDto>();

        }
    }
}
