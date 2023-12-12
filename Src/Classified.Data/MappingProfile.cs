using AutoMapper;
using Classified.Domain.Entities;
using Classified.Domain.Entities.Dtos.AdimDtos;
using Classified.Domain.ViewModels.Advertisment;
using Classified.Domain.ViewModels.Image;

namespace Classified.Data
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Mapping ClassifiedCategory with CategoryViewMoels
            Mapper.CreateMap<ClassifiedCategory, CategoryViewMoel>();
            Mapper.CreateMap<CategoryViewMoel, ClassifiedCategory>();
            #endregion

            #region Mapping ClassifiedCategoryAttribute with CategoryAttributesViewModel

            Mapper.CreateMap<ClassifiedCategoryAttribute, CategoryAttributesViewModel>();
            Mapper.CreateMap<CategoryAttributesViewModel, ClassifiedCategoryAttribute>();

            Mapper.CreateMap<CategoryAttributesViewModel, CategoryAttributesDto>();
            Mapper.CreateMap<CategoryAttributesDto, CategoryAttributesViewModel>();

            #endregion

            #region Mapping ClassifiedCategoryAttributeValues with CategoryAttribueValuesViewModel

            Mapper.CreateMap<ClassifiedCategoryAttributeValue, CategoryAttributeValuesViewModel>();
            Mapper.CreateMap<CategoryAttributeValuesViewModel, ClassifiedCategoryAttributeValue>();


            #endregion

            #region Mapping Classified Advertisement with Advertisement View Models

            Mapper.CreateMap<AdvertisementEmailBasePrimaryRegisterationViewModel, ClassifiedAdvertisement>();
            Mapper.CreateMap<ClassifiedAdvertisement, AdvertisementEmailBasePrimaryRegisterationViewModel>();

            Mapper.CreateMap<AdvertisementConfirmationViewModel,
                ClassifiedAdvertisementConfirmation>();
            Mapper.CreateMap<ClassifiedAdvertisementConfirmation,
                AdvertisementConfirmationViewModel>();

            Mapper.CreateMap<ClassifiedAdvertisementViewModel, ClassifiedAdvertisement>();
            Mapper.CreateMap<ClassifiedAdvertisement, ClassifiedAdvertisementViewModel>();

            Mapper.CreateMap<ClassifiedAdvertisementViewModelClient, ClassifiedAdvertisement>();
            Mapper.CreateMap<ClassifiedAdvertisement, ClassifiedAdvertisementViewModelClient>();


            Mapper.CreateMap<ClassifiedAdvertisementModifyViewModel, ClassifiedAdvertisement>();
            Mapper.CreateMap<ClassifiedAdvertisement, ClassifiedAdvertisementModifyViewModel>();

            Mapper.CreateMap<AdvertisementAttributesViewModel, ClassifiedCategoryAttribute>();
            Mapper.CreateMap<ClassifiedCategoryAttribute, AdvertisementAttributesViewModel>();


            Mapper.CreateMap<AdvertisementAttributesValueViewModel, ClassifiedAdvertisementAttribute>();
            Mapper.CreateMap<ClassifiedAdvertisementAttribute, AdvertisementAttributesValueViewModel>();

            Mapper.CreateMap<AdvertisementAttributesViewModel, FunctionClassifiedAdvertisementAttribute>();
            Mapper.CreateMap<FunctionClassifiedAdvertisementAttribute, AdvertisementAttributesViewModel>();

            Mapper.CreateMap<func_FetchClassifiedAdvertisements, func_FetchClassifiedAdvertisementsViewModel>();
            Mapper.CreateMap<func_FetchClassifiedAdvertisementsViewModel, func_FetchClassifiedAdvertisements>();


            #endregion

            #region Mapping Classified Images With Images View Model

            Mapper.CreateMap<ImageViewModel, ClassifiedImages>();
            Mapper.CreateMap<ClassifiedImages, ImageViewModel>();

            #endregion


            #region Classified Advertisements Rejection Mapping


            Mapper.CreateMap<ClassifiedAdvertisementRejectCommentViewModel, ClassifiedAdvertisementRejectComment>();
            Mapper.CreateMap<ClassifiedAdvertisementRejectComment, ClassifiedAdvertisementRejectCommentViewModel>();


            #endregion
        }
    }
}