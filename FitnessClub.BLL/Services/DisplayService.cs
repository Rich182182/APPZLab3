using System.Collections.Generic;
using AutoMapper;
using FitnessClub.BLL.DTOs;
using FitnessClub.BLL.Services.Interfaces;
using FitnessClub.DAL.Repositories.Interfaces;

namespace FitnessClub.BLL.Services
{
    public class DisplayService : IDisplayService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public DisplayService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public IEnumerable<ClubDto> GetClubs() => 
            _mapper.Map<IEnumerable<ClubDto>>(_uow.Clubs.GetAll());

        public IEnumerable<TrainingDto> GetTrainings() => 
            _mapper.Map<IEnumerable<TrainingDto>>(_uow.Trainings.GetAll());

        public IEnumerable<SubscriptionTypeDto> GetSubscriptionTypes() => 
            _mapper.Map<IEnumerable<SubscriptionTypeDto>>(_uow.SubscriptionTypes.GetAll());
    }
}