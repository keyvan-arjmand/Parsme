using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;
using Domain.Entity.Product;

namespace Domain.Entity.IndexPage;

public class BrandLanding : BaseEntity
{
    public int BrandId { get; set; }

    [ForeignKey("BrandId")] public Brand? Brand { get; set; }

    //slider
    public string? ImageSlider { get; set; }
    public string? HrefSlider { get; set; }
    public string? TitleSlider { get; set; }
    public string? DescSlider { get; set; }

    public string? ImageSlider2 { get; set; }
    public string? HrefSlider2 { get; set; }
    public string? TitleSlider2 { get; set; }
    public string? DescSlider2 { get; set; }

    public string? ImageSlider3 { get; set; }
    public string? HrefSlider3 { get; set; }
    public string? TitleSlider3 { get; set; }
    public string? DescSlider3 { get; set; }

    public string? ImageSlider4 { get; set; }
    public string? HrefSlider4 { get; set; }
    public string? TitleSlider4 { get; set; }
    public string? DescSlider4 { get; set; }

    public string? ImageSlider5 { get; set; }
    public string? HrefSlider5 { get; set; }
    public string? TitleSlider5 { get; set; }
    public string? DescSlider5 { get; set; }

    //banner

    public string? BigBanner { get; set; }
    public string? HrefBigBanner { get; set; }
    public string? SmallBanner1 { get; set; }
    public string? HrefSmallBanner1 { get; set; }
    public string? SmallBanner2 { get; set; }
    public string? HrefSmallBanner2 { get; set; }
    public string? SmallBanner3 { get; set; }
    public string? HrefSmallBanner3 { get; set; }
    public string? SmallBanner4 { get; set; }
    public string? HrefSmallBanner4 { get; set; }
}