using jellyfin_blazor.Data;
using jellyfin_blazor.Services;
using Microsoft.AspNetCore.Components;

namespace jellyfin_blazor.Components
{
    public partial class ItemCard
    {
        [Parameter]
        public dynamic Data { get; set; }
        private HttpService _httpService;

        public ItemCard(HttpService httpService)
        {
            _httpService = httpService;
        }
        private string _imageUrl
        {
            get
            {
                return $"{_httpService.BaseAddress}/Items/{Data.Id}/Images/Primary?fillHeight=300&fillWidth=200";
            }
        }
    }
}
