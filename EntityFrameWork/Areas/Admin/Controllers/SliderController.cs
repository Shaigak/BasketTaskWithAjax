using EntityFrameWork.Data;
using EntityFrameWork.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameWork.Areas.Admin.Controllers;

[Area("Admin")]
public class SliderController : Controller

{

    private readonly AppDbContext _Context;

    public SliderController(AppDbContext Context)
    {
        _Context = Context;
    }
    public async Task<IActionResult> Index()
    {
        IEnumerable<Slider> sliders = await _Context.Sliders.Where(m => !m.SoftDelete).ToListAsync();
        return View(sliders);
    }


    [HttpGet]
    public async Task<IActionResult> Detail(int? id)
    {

        if (id == null) return BadRequest();

        Slider? slider = await _Context.Sliders.FirstOrDefaultAsync(m => m.Id == id);

        if (slider is null) return NotFound();

        return View(slider);

    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }


    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return BadRequest();

        Slider? slider = await _Context.Sliders.FirstOrDefaultAsync(m => m.Id == id);

        if (slider is null) return NotFound();

        return View(slider);
    }






}

