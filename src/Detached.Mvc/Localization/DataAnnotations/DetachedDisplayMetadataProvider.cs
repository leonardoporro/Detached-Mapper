using Detached.Mvc.Localization.Mapping;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.Extensions.Localization;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Detached.Mvc.Localization.DataAnnotations
{
    public class DetachedDisplayMetadataProvider : IDisplayMetadataProvider
    {
        #region Fields

        IStringLocalizerFactory _stringLocalizerFactory;
        IResourceMapper _resourceMapper;

        #endregion

        #region Ctor.

        public DetachedDisplayMetadataProvider(IResourceMapper resourceMapper, IStringLocalizerFactory stringLocalizerFactory)
        {
            _resourceMapper = resourceMapper;
            _stringLocalizerFactory = stringLocalizerFactory;
        }

        #endregion

        public void CreateDisplayMetadata(DisplayMetadataProviderContext context)
        { 
            switch (context.Key.MetadataKind)
            {
                case ModelMetadataKind.Property:
                    if (context.DisplayMetadata.DisplayName == null)
                    {
                        ResourceKey displayKey = _resourceMapper.GetKey(context.Key.ContainerType.Namespace, context.Key.ContainerType.Name, context.Key.Name, "DisplayName");
                        if (displayKey != null)
                        {
                            IStringLocalizer localizer = _stringLocalizerFactory.Create(displayKey.ResourceName, displayKey.ResourceLocation);
                            context.DisplayMetadata.DisplayName = () => localizer.GetString(displayKey.KeyName);
                        }
                    }
                    break;
                case ModelMetadataKind.Type:
                    break;
            }
        }
    }
}
