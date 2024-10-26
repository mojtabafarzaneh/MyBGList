﻿using Microsoft.AspNetCore.Mvc;

namespace MyBGListApi.Controllers;

[ApiController]
public class ErrorController: ControllerBase
{
    [Route("/error")]
    [HttpGet]
    public IActionResult Error()
    {
        return Problem();
    }
    
}