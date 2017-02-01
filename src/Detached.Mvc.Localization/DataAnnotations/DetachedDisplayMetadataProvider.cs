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
                        ResourceKey displayKey = _resourceMapper.GetFieldKey(context.Key.ContainerType, context.Key.Name, "DisplayName");
                        if (displayKey != null)
                        {
                            IStringLocalizer localizer = _stringLocalizerFactory.Create(displayKey.Source, displayKey.Location);
                            context.DisplayMetadata.DisplayName = () => localizer.GetString(displayKey.Name);
                        }
                    }
                    break;
                case ModelMetadataKind.Type:
                    break;
            }
        }
    }
}
