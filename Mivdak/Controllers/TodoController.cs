using Microsoft.AspNetCore.Mvc;
using Mivdak.Models;
using System.Text;
using System;
using System.Text.Json;

namespace Mivdak.Controllers
{
    public class TodoController(IHttpClientFactory clientFactory) : Controller
    {

        public async Task<IActionResult> Index()
        {
            var httpClient = clientFactory.CreateClient();
            var res = await httpClient.GetAsync("https://dummyjson.com/todos");
            if (res.IsSuccessStatusCode)
            {
                var content = await res.Content.ReadAsStringAsync();
                TodosModel? todos = JsonSerializer.Deserialize<TodosModel>(content,new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                return View(todos!.todos);
            }
            return RedirectToAction("Index", "Home");

        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(TodoModel todo)
        {
            var post = JsonContent.Create(todo);
            var httpClient = clientFactory.CreateClient();
            var res = await httpClient.PostAsync("https://dummyjson.com/todos/add", post);
            if (res.IsSuccessStatusCode)
            {
                var content = await res.Content.ReadAsStringAsync();
                return View(res.StatusCode);
            }
            return RedirectToAction("Index", "Home");

        }
        [HttpPut]
        public async Task<IActionResult> UpdatePut(int id, TodoModel todo)
        {
            var put = new StringContent(JsonSerializer.Serialize(todo), Encoding.UTF8, "application/json");
            var httpClient = clientFactory.CreateClient();
            var res = await httpClient.PutAsync($"https://jsonplaceholder.typicode.com/posts/{id}", put);
            if (res.IsSuccessStatusCode)
            {
                var content = await res.Content.ReadAsStringAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var httpClient = clientFactory.CreateClient();
            var res = await httpClient.DeleteAsync($"https://jsonplaceholder.typicode.com/posts/{id}");
            if (res.IsSuccessStatusCode)
            {
                var content = await res.Content.ReadAsStringAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }






    }
}

