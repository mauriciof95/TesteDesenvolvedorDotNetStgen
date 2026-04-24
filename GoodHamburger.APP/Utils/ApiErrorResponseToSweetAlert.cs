using System.Net.Http.Json;

namespace GoodHamburger.App.Utils
{
    public class ApiErrorResponseToSweetAlert
    {
        private HttpResponseMessage _response;

        public ApiErrorResponseToSweetAlert(HttpResponseMessage response)
        { _response = response; }

        public async Task<SweetAlertData> Alert(string message = "")
        {
            string alertMessage = message;

            if ((int)_response.StatusCode >= 400)
            {
                var apiErrorResponse = await _response.Content.ReadFromJsonAsync<ApiErrorResponse>();
                alertMessage = apiErrorResponse.Error ?? message;
            }
            return new SweetAlertData(_response.StatusCode, alertMessage);
        }

        public bool Success()
        => (int) _response.StatusCode >= 200 && (int) _response.StatusCode <= 299;

    }
}
