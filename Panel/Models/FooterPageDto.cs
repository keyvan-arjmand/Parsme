namespace Panel.Models;

public class FooterPageDto
{
     public string WhyParsDesc { get; set; } = string.Empty;
    public string SeoWhyParsTitle { get; set; } = string.Empty;
    public string SeoWhyParsDesc { get; set; } = string.Empty;

    public string SeoWhyParsCanonical { get; set; } = string.Empty;
    public IFormFile SeoWhyParsImage { get; set; } 

    //در یک نگاه
    public string ParsDarYekDesc { get; set; } = string.Empty;
    public string SeoParsDarYekTitle { get; set; } = string.Empty;
    public string SeoParsDarYekDesc { get; set; } = string.Empty;

    public string SeoParsDarYekCanonical { get; set; } = string.Empty;
    public IFormFile SeoParsDarYekImage { get; set; } 

    //اهداف
    public string ParsGoalsDesc { get; set; } = string.Empty;
    public IFormFile ParsGoalsImage { get; set; } 
    public string SeoParsGoalsTitle { get; set; } = string.Empty;
    public string SeoParsGoalsDesc { get; set; } = string.Empty;

    public string SeoParsGoalsCanonical { get; set; } = string.Empty;

    //خرید اقساطی
    public string ParsInstallmentPurchaseDesc { get; set; } = string.Empty;
    public IFormFile ParsInstallmentPurchaseImage { get; set; }  
    public string SeoParsInstallmentPurchaseTitle { get; set; } = string.Empty;
    public string SeoParsInstallmentPurchaseDesc { get; set; } = string.Empty;

    public string SeoParsInstallmentPurchaseCanonical { get; set; } = string.Empty;

    //راهنما خرید 
    public string ParsBuyingGuideDesc { get; set; } = string.Empty;
    public IFormFile ParsBuyingGuideImage{ get; set; } 
    public string SeoParsBuyingGuideTitle { get; set; } = string.Empty;
    public string SeoParsBuyingGuideDesc { get; set; } = string.Empty;

    public string SeoParsBuyingGuideCanonical { get; set; } = string.Empty;

    // خرید سازمانی
    public string ParsOrganizationalPurchaseDesc { get; set; } = string.Empty;
    public IFormFile ParsOrganizationalPurchaseImage { get; set; }  
    public string SeoParsOrganizationalPurchaseTitle { get; set; } = string.Empty;
    public string SeoParsOrganizationalPurchaseDesc { get; set; } = string.Empty;

    public string SeoParsOrganizationalPurchaseCanonical { get; set; } = string.Empty;

    // ضمانت خرید
    public string ParsWarrantyDesc { get; set; } = string.Empty;
    public IFormFile ParsWarrantyDescImage { get; set; }  
    public string SeoParsWarrantyTitle { get; set; } = string.Empty;
    public string SeoParsWarrantyDesc { get; set; } = string.Empty;
    public string SeoParsWarrantyCanonical { get; set; } = string.Empty;

    //شیوه ها و هزینه های ارسال
    public string ParsShippingMethodsDesc { get; set; } = string.Empty;
    public IFormFile ParsShippingMethodsDescImage { get; set; } 
    public string SeoParsShippingMethodsTitle { get; set; } = string.Empty;
    public string SeoParsShippingMethodsDesc { get; set; } = string.Empty;
    public string SeoParsShippingMethodsCanonical { get; set; } = string.Empty;
    //مشاوره قبل از خرید
    public string ParsConsultationBeforePurchaseDesc { get; set; } = string.Empty;
    public IFormFile ParsConsultationBeforePurchaseDescImage { get; set; }  
    public string SeoParsConsultationBeforePurchaseTitle { get; set; } = string.Empty;
    public string SeoParsConsultationBeforePurchaseDesc { get; set; } = string.Empty;
    public string SeoParsConsultationBeforePurchaseCanonical { get; set; } = string.Empty;  
    //رویه های بازگرداندن کالا
    public IFormFile ParsProceduresForReturningGoodsDescImage { get; set; }  
    public string ParsProceduresForReturningGoodsDesc { get; set; } = string.Empty;
    public string SeoParsProceduresForReturningGoodsTitle { get; set; } = string.Empty;
    public string SeoParsProceduresForReturningGoodsDesc { get; set; } = string.Empty;
    public string SeoParsProceduresForReturningGoodsCanonical { get; set; } = string.Empty;
    //ره گیری سفارش های ارسالی
    public string ParsTrackingOrdersDesc { get; set; } = string.Empty;
    public IFormFile ParsTrackingOrdersDescImage { get; set; }  
    public string SeoParsTrackingOrdersGoodsTitle { get; set; } = string.Empty;
    public string SeoParsTrackingOrdersGoodsDesc { get; set; } = string.Empty;
    public string SeoParsTrackingOrdersGoodsCanonical { get; set; } = string.Empty;  
    //Online support
    public string ParsOnlineSupportDesc { get; set; } = string.Empty;
    public IFormFile ParsOnlineSupportDescImage { get; set; } 
    public string SeoParsOnlineSupportTitle { get; set; } = string.Empty;
    public string SeoParsOnlineSupportDesc { get; set; } = string.Empty;
    public string SeoParsOnlineSupportCanonical { get; set; } = string.Empty;
    //فوانین و مقررات
    public string ParsRegulationsDesc { get; set; } = string.Empty;
    public IFormFile ParsRegulationsDescImage { get; set; } 
    public string SeoParsRegulationsTitle { get; set; } = string.Empty;
    public string SeoParsRegulationsDesc { get; set; } = string.Empty;
    public string SeoParsRegulationsCanonical { get; set; } = string.Empty;
    //حریم خصوصی کاربران
    public string ParsUserPrivacyDesc { get; set; } = string.Empty;
    public IFormFile ParsUserPrivacyDescImage { get; set; } 
    public string SeoParsUserPrivacyTitle { get; set; } = string.Empty;
    public string SeoParsUserPrivacyDesc { get; set; } = string.Empty;
    public string SeoParsUserPrivacyCanonical { get; set; } = string.Empty;
}