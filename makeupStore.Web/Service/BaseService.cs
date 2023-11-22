using System.Net;
using System.Text;
using makeupStore.Web.Models;
using makeupStore.Web.Service.IService;
using makeupStore.Web.Utility;
using Newtonsoft.Json;

namespace makeupStore.Web.Service;

public class BaseService : IBaseService
{

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ITokenProvider _tokenProvider;
    public BaseService(IHttpClientFactory httpClientFactory, ITokenProvider tokenProvider)
    {
        _httpClientFactory = httpClientFactory;
        _tokenProvider = tokenProvider;
    }
    public async Task<ResponseDto?> SendAsync(RequestDto requestDto, bool withBearer = true)
    {
        try
        {
            HttpClient client = _httpClientFactory.CreateClient("makeupStoreAPI");
            HttpRequestMessage message = new HttpRequestMessage();
            
            message.Headers.Add("Accept", "application/json");

            if (withBearer)
            {
                var token = _tokenProvider.GetToken();
                message.Headers.Add("Authorization", $"Bearer {token}");
            }
            message.RequestUri = new Uri(requestDto.Url);
            if (requestDto.Data != null)
            {
                message.Content = new StringContent(JsonConvert.SerializeObject(requestDto.Data), Encoding.UTF8, "application/json");
                Console.WriteLine("m c!!!!! "+message.Content.ToString());
                Console.WriteLine(JsonConvert.SerializeObject(requestDto.Data));
            }

            HttpResponseMessage? apiResponse = null;
            switch (requestDto.ApiType)
            {
                case SD.ApiType.POST:
                {
                    message.Method = HttpMethod.Post;
                    break;
                }
                case SD.ApiType.PUT:
                {
                    message.Method = HttpMethod.Put;
                    break;
                }
                case SD.ApiType.DELETE:
                {
                    message.Method = HttpMethod.Delete;
                    break;
                }
                default:
                {
                    message.Method = HttpMethod.Get;
                    break;
                }
            }

            apiResponse = await client.SendAsync(message);
            switch (apiResponse.StatusCode)
            {
                case HttpStatusCode.NotFound:
                {
                    return new ResponseDto()
                    {
                        IsSuccess = false,
                        Message = "Not Found"
                    };
                }
                case HttpStatusCode.Forbidden:
                {
                    return new ResponseDto()
                    {
                        IsSuccess = false,
                        Message = "Forbidden"
                    };
                }
                case HttpStatusCode.Unauthorized:
                {
                    return new ResponseDto()
                    {
                        IsSuccess = false,
                        Message = "Unauthorized"
                    };
                }
                case HttpStatusCode.InternalServerError:
                {
                    return new ResponseDto()
                    {
                        IsSuccess = false,
                        Message = "InternalServerError"
                    };
                }
                default:
                {
                    var apiContent = await apiResponse.Content.ReadAsStringAsync();
                    var apiResponseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
                    return apiResponseDto;
                }
            }
        }
        catch (Exception ex)
        {
            var dto = new ResponseDto
            {
                Message = ex.Message,
                IsSuccess = false,
            };
            return dto;
        }
    }
}