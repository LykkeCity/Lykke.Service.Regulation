// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Lykke.Service.Regulation.Client.AutorestClient.Models
{
    using Microsoft.Rest;
    using Newtonsoft.Json;
    using System.Linq;

    public partial class NewRegulationModel
    {
        /// <summary>
        /// Initializes a new instance of the NewRegulationModel class.
        /// </summary>
        public NewRegulationModel()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the NewRegulationModel class.
        /// </summary>
        public NewRegulationModel(string id, string profileType)
        {
            Id = id;
            ProfileType = profileType;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Id")]
        public string Id { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "ProfileType")]
        public string ProfileType { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (Id == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "Id");
            }
            if (ProfileType == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "ProfileType");
            }
        }
    }
}
