using Common.Data;
using Common.Models;
using Taekwondo.Data;
using Taekwondo.Data.Entities;

namespace Taekwondo.Service
{
    public class TrainingOrganizationService:BaseService
    {
	    private readonly TrainingOrganizationRepository _trainingOrganizationRepository;
		public TrainingOrganizationService(TrainingOrganizationRepository trainingOrganizationRepository,DataBaseContext dbContext) : base(dbContext)
		{
			_trainingOrganizationRepository = trainingOrganizationRepository;
		}

	    public TrainingOrganization Get(long trainingOrganizationId)
	    {
		    var trainingOrganization= _trainingOrganizationRepository.Get(trainingOrganizationId);
		    if (trainingOrganization == null)
		    {
				throw new PlatformException("场馆信息不能为空");
		    }
		    return trainingOrganization;
	    }

	    public QueryResult<TrainingOrganization> Query(TrainingOrganizationQuery query)
	    {
		    return _trainingOrganizationRepository.Query(query);

	    }

	    public void Add(TrainingOrganization organization)
	    {
		    _trainingOrganizationRepository.Insert(organization);
	    }


	    public void Update(TrainingOrganization organization)
	    {
		    _trainingOrganizationRepository.Update(organization);
	    }

	    public void Delete(long id)
	    {
			_trainingOrganizationRepository.Delete(id);
	    }
    }
}
