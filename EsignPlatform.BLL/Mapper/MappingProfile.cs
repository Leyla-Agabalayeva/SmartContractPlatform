using AutoMapper;
using EsignPlatform.BLL.DTOs.Contract;
using EsignPlatform.BLL.DTOs.Template;
using EsignPlatform.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsignPlatform.BLL.Mapper
{

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Template
            CreateMap<Template, TemplateListDto>();

            // Contract -> list
            CreateMap<Contract, ContractListDto>()
                .ForMember(d => d.TemplateName, o => o.MapFrom(s => s.Template.Name))
                .ForMember(d => d.PartiesCount, o => o.MapFrom(s => s.Parties.Count))
                .ForMember(d => d.SignedCount, o => o.MapFrom(s => s.Parties.Count(p => p.Signed)));

            // ContractParty
            CreateMap<ContractParty, ContractPartyDto>();

            // Note: TemplateDetailDto.Fields və ContractDetailDto (JSON parse tələb edir)
            // service içində manual doldurulur.
        }
    }

}
