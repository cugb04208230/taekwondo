using System;
using System.Collections.Generic;
using System.Text;
using Taekwondo.Data.Entities;

namespace Taekwondo.Service
{
    public class TrainingOrganizationEntService
    {

        TrainingOrganizationEntRepository _trainingOrganizationEntRepository;
        public TrainingOrganizationEntService(TrainingOrganizationEntRepository trainingOrganizationEntRepository)
        {
            _trainingOrganizationEntRepository = trainingOrganizationEntRepository;
        }

        public TrainingOrganizationEnt GetEntByManagerId(long managerid)
        {
            var trainingOrganizationEnt = _trainingOrganizationEntRepository.FirstOrDefault(o => o.ManagerId == managerid);
            return trainingOrganizationEnt;
        }

    }
}
