using Microsoft.AspNetCore.Http;

namespace Application.Dtos.Client;

public class BrandLandingDto
{
    public int Id { get; set; }

    public int BrandTagId { get; set; }

    //slider
    public IFormFile? ImageSlider { get; set; }
    public string? HrefSlider { get; set; }
    public string? TitleSlider { get; set; }
    public string? DescSlider { get; set; }

    public IFormFile? ImageSlider2 { get; set; }
    public string? HrefSlider2 { get; set; }
    public string? TitleSlider2 { get; set; }
    public string? DescSlider2 { get; set; }

    public IFormFile? ImageSlider3 { get; set; }
    public string? HrefSlider3 { get; set; }
    public string? TitleSlider3 { get; set; }
    public string? DescSlider3 { get; set; }

    public IFormFile? ImageSlider4 { get; set; }
    public string? HrefSlider4 { get; set; }
    public string? TitleSlider4 { get; set; }
    public string? DescSlider4 { get; set; }

    public IFormFile? ImageSlider5 { get; set; }
    public string? HrefSlider5 { get; set; }
    public string? TitleSlider5 { get; set; }
    public string? DescSlider5 { get; set; }

    //banner
    public IFormFile? BigBanner { get; set; }
    public string? HrefBigBanner { get; set; }
    public IFormFile? SmallBanner1 { get; set; }
    public string? HrefSmallBanner1 { get; set; }
    public IFormFile? SmallBanner2 { get; set; }
    public string? HrefSmallBanner2 { get; set; }
    public IFormFile? SmallBanner3 { get; set; }
    public string? HrefSmallBanner3 { get; set; }
    public IFormFile? SmallBanner4 { get; set; }
    public string? HrefSmallBanner4 { get; set; }
}