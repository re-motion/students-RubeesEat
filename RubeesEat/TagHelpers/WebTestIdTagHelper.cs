using Microsoft.AspNetCore.Razor.TagHelpers;

namespace RubeesEat.TagHelpers;

[HtmlTargetElement(Attributes = "t-id")]
public class WebTestIdTagHelper : TagHelper
{ 
    private readonly IWebHostEnvironment _environment;
    
    [HtmlAttributeName("t-id")] 
    public string Id { get; set; }

    public WebTestIdTagHelper(IWebHostEnvironment environment)
    {
        _environment = environment;
    }
    
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (_environment.IsProduction())
        {
            return;
        }
        output.Attributes.Add("data-test-id", Id);
    }
}
