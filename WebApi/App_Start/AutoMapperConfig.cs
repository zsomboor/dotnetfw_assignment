using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.DTO;
using WebApi.Models;

namespace WebApi.App_Start
{
    public class AutoMapperConfig
    {
        public static void Initialize()
        {
            Mapper.Initialize((config) =>
            {
                config.CreateMap<Patient, PatientBaseDTO>().ReverseMap();
                config.CreateMap<CheckIn, CheckInDTO>().ReverseMap();
                config.CreateMap<HistoryEntry, HistoryEntryDTO>().ReverseMap();
            });
        }
    }
}