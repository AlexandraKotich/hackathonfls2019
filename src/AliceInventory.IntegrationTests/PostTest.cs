using System;
using Xunit;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AliceInventory;
using Newtonsoft.Json;
using Xunit.Extensions.Ordering;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
[assembly: TestCaseOrderer("Xunit.Extensions.Ordering.TestCaseOrderer", "Xunit.Extensions.Ordering")]
[assembly: TestCollectionOrderer("Xunit.Extensions.Ordering.CollectionOrderer", "Xunit.Extensions.Ordering")]
namespace AliceInventory.IntegrationTests
{
    [Order(1)]
    public class PostTest
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public PostTest()
        {
            _server = new TestServer(WebHost.CreateDefaultBuilder().UseStartup<AliceInventory.Startup>());
            _client = _server.CreateClient();
        }

        //private string jsonExample = "{\"meta\":{\"locale\":\"ru-RU\",\"timezone\":\"Europe/Moscow\",\"client_id\": \"ru.yandex.searchplugin/5.80 (Samsung Galaxy; Android 4.4)\",    \"interfaces\": {      \"screen\": { }    }  },  \"request\": {    \"command\": \"*TEXT*\",    \"original_utterance\": \"������ ����� �� ����� ���� ��������, 16 �� ������\",    \"type\": \"SimpleUtterance\",    \"markup\": {      \"dangerous_context\": true    },    \"payload\": {},    \"nlu\": {      \"tokens\": [        \"������\",        \"�����\",        \"��\",        \"����\",        \"��������\",        \"16\",        \"��\",        \"������\"      ],      \"entities\": [        {          \"tokens\": {            \"start\": 2,            \"end\": 6          },          \"type\": \"YANDEX.GEO\",          \"value\": {            \"house_number\": \"16\",            \"street\": \"���� ��������\"          }        },        {          \"tokens\": {            \"start\": 3,            \"end\": 5          },          \"type\": \"YANDEX.FIO\",          \"value\": {            \"first_name\": \"���\",            \"last_name\": \"�������\"          }        },        {          \"tokens\": {            \"start\": 5,            \"end\": 6          },          \"type\": \"YANDEX.NUMBER\",          \"value\": 16        },        {          \"tokens\": {            \"start\": 6,            \"end\": 8          },          \"type\": \"YANDEX.DATETIME\",          \"value\": {            \"day\": 1,            \"day_is_relative\": true          }        }      ]    }  },  \"session\": {    \"new\": true,    \"message_id\": 4,    \"session_id\": \"2eac4854-fce721f3-b845abba-20d60\",    \"skill_id\": \"3ad36498-f5rd-4079-a14b-788652932056\",    \"user_id\": \"AC9WC3DF6FCE052E45A4566A48E6B7193774B84814CE49A922E163B8B29881DC\"  },  \"version\": \"1.0\"}";

        private StringContent CreateJsonContent(string text)
        {
            AliceRequest response = new AliceRequest() { Request = new RequestModel() { Command = text }};
            string json = JsonConvert.SerializeObject(response);
            return new StringContent(json, Encoding.Default, "application/json");
        }
        
        [Theory, Order(1)]
        [InlineData("������ 3 ������", new []{ "������ ����� 3 ��" })]
        [InlineData("������ 1 ������", new []{ "������ ����� 4 ��" })]
        [InlineData("��� 7 �����", new []{ "������ ���� 7 ��" })]
        [InlineData("������� 8 ������ ����", new []{ "������ ���� 8 ������" })]
        public async Task Adding(string request, string[] responces)
        {
            var content = CreateJsonContent(request);
            var result = await _client.PostAsync("/api/values", content);
            var jsonResult = await result.Content.ReadAsStringAsync();
            var aliceAnswer = JsonConvert.DeserializeObject<AliceResponse>(jsonResult).Response.Text;

            Assert.Contains(responces, expected => expected == aliceAnswer);
        }

        [Theory, Order(2)]
        [InlineData("����� 2 ������", new []{ "������ ����� 2 ��" })]
        [InlineData("����� 1 �����", new []{ "������ ���� 6 ��" })]
        [InlineData("����� 1 ���� ����", new []{ "������ ���� 7 ������" })]
        public async Task Removing(string request, string[] responces)
        {
            var content = CreateJsonContent(request);
            var result = await _client.PostAsync("/api/values", content);
            var jsonResult = await result.Content.ReadAsStringAsync();
            var aliceAnswer = JsonConvert.DeserializeObject<AliceResponse>(jsonResult).Response.Text;

            Assert.Contains(responces, expected => expected == aliceAnswer);
        }
        [Theory, Order(3)]
        [InlineData("������ ����� 3", new []{ "������ ����� 3 ��" })]
        [InlineData("������ ���� 5", new []{ "������ ���� 5 ��" })]
        [InlineData("������ ���� 3 �����", new []{ "������ ���� 3 �" })]
        [InlineData("������ ����� 3 ����������", new []{ "������ ����� 3 ��" })]
        public async Task Setting(string request, string[] responces)
        {
            var content = CreateJsonContent(request);
            var result = await _client.PostAsync("/api/values", content);
            var jsonResult = await result.Content.ReadAsStringAsync();
            var aliceAnswer = JsonConvert.DeserializeObject<AliceResponse>(jsonResult).Response.Text;

            Assert.Contains(responces, expected => expected == aliceAnswer);
        }

        [Theory, Order(4)]
        [InlineData("������� �����", new []{ "�����: 3 ��" })]
        [InlineData("������� ����", new []{ "����: 3 �" })]
        [InlineData("������� �����", new []{ "������: 3 ��" })]
        public async Task Showing(string request, string[] responces)
        {
            var content = CreateJsonContent(request);
            var result = await _client.PostAsync("/api/values", content);
            var jsonResult = await result.Content.ReadAsStringAsync();
            var aliceAnswer = JsonConvert.DeserializeObject<AliceResponse>(jsonResult).Response.Text;

            Assert.Contains(responces, expected => expected == aliceAnswer);
        }

        [Theory, Order(5)]
        [InlineData("������ �����", new []{ "�����:\n������: 3 ��\n�����: 5 ��\n����: 3 �\n�����: 3 ��" })]
        public async Task ShowingAll(string request, string[] responces)
        {
            var content = CreateJsonContent(request);
            var result = await _client.PostAsync("/api/values", content);
            var jsonResult = await result.Content.ReadAsStringAsync();
            var aliceAnswer = JsonConvert.DeserializeObject<AliceResponse>(jsonResult).Response.Text;

            Assert.Contains(responces, expected => expected == aliceAnswer);
        }
    }
}
