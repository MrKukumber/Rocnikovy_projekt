using System.IO;
using System.Threading;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.UserModelMan.UserModels;
using Optepafi.Models.Utils;
using Optepafi.Models.Utils.Configurations;

namespace Optepafi.Models.UserModelMan.UserModelReps;

/// <summary>
/// One of three interfaces whose implementations represent properties of individual user model types that is tied to specific template type.
/// 
/// The other two are <see cref="IUserModelIdentifier{TUserModel}"/> and <see cref="IUserModelTemplateBond{TTemplate}"/>.  
/// This interface provides methods and properties, that are used for creating and deserializing user models. It also contains referenced to template to which is represented user model tied.  
/// It should not be implemented right away. All implementations should derive from <see cref="UserModelRepresentative{TUserModel,TTemplate,TConfiguration}"/> instead.  
/// Thanks to covariance of its type parameters it is useful for transferring of user model representatives in non generic way.  
/// </summary>
/// <typeparam name="TTemplate">Template type to which represented user model is tied.</typeparam>
/// <typeparam name="TUserModel">Type of represented user model.</typeparam>
public interface IUserModelType<out TUserModel, out TTemplate> 
    where TUserModel : IUserModel<TTemplate> where TTemplate : ITemplate 
{
    string UserModelTypeName { get; }
    
    /// <summary>
    /// Represents suffix of file name to which should be eventual serialization saved.
    /// 
    /// Suffix should represent type of user model, so it could be easily identified and deserialized.  
    /// It should be unique for every user model type.  
    /// </summary>
    string UserModelFileNameSuffix { get; }
    
    /// <summary>
    /// Extension of file format, which is user model serialized to.
    /// </summary>
    string UserModelFileExtension { get; }
    
    /// <summary>
    /// Template to which represented user model is tied to.
    /// </summary>
    TTemplate AssociatedTemplate { get; }
    
    /// <summary>
    /// Provides default configuration for represented model.
    /// </summary>
    IConfiguration DefaultConfigurationDeepCopy { get; }
    
    /// <summary>
    /// Returns new instance of user model represented by this user model type.
    /// </summary>
    /// <param name="configuration">Configuration used in newly created user model.</param>
    /// <returns>New instance of user model.</returns>
    TUserModel GetNewUserModel(IConfiguration? configuration);
    
    /// <summary>
    /// Tries to deserialize user model from provided stream.
    /// </summary>
    /// <param name="serializationWithPath">Provided stream from which user model should be deserialized alongside with path to the serialization file.</param>
    /// <param name="configuration">Configuration which should be used in deserialized user model.</param>
    /// <param name="cancellationToken">Token for cancellation of deserialization. Provided, when the deserialization is not needed anymore.</param>
    /// <param name="result">Out parameter for result of deserialization.</param>
    /// <returns>Resulting deserialized user model.</returns>
    TUserModel? DeserializeUserModel((Stream,string) serializationWithPath, IConfiguration? configuration, CancellationToken? cancellationToken, out UserModelManager.UserModelLoadResult result);
}