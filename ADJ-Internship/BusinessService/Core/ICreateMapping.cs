using AutoMapper;

namespace ADJ.BusinessService.Core
{
    public interface ICreateMapping
    {
        /// <summary>
        /// Defines the AutoMapper mapping and adds it into the mapping profile
        /// </summary>
        /// <param name="profile"></param>
        void CreateMapping(Profile profile);
    }
}
