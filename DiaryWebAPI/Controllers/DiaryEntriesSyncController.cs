using DiaryWebAPI.Data;
using DiaryWebAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DiaryWebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DiaryEntriesSyncController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;

    public DiaryEntriesSyncController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // GET: api/DiaryEntries
    [HttpGet]
    public IEnumerable<DiaryEntry> GetDiaryEntries()
    {
        return _dbContext.DiaryEntries.ToList();
    }

    // GET: api/DiaryEntries/5
    [HttpGet("{id}")]
    public ActionResult<DiaryEntry> GetDiaryEntry(int id)
    {
        var diaryEntry = _dbContext.DiaryEntries.Find(id);
        if (diaryEntry == null)
        {
            return NotFound();
        }
        return diaryEntry;
    }

    [HttpPost]
    public ActionResult<DiaryEntry> CreateDiaryEntry(DiaryEntry diaryEntry) {
        diaryEntry.Id = 0; // To allow DB assign the proper Id
        _dbContext.DiaryEntries.Add(diaryEntry);
        _dbContext.SaveChanges();

        // Other option to return the similar in location section at response.
        //var resourceUrl = Url.Action(nameof(GetDiaryEntries), new {id = diaryEntry.Id });
        //return Created(resourceUrl, diaryEntry);

        return CreatedAtAction("GetDiaryEntry", new { id = diaryEntry.Id }, diaryEntry);
    }


    // PUT: api/DiaryEntries/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    //public IActionResult PutDiaryEntry(int id, [FromBody] DiaryEntry diaryEntry)
    //FromBody puede ir o no, pero lo demas se toma de ahi.
    public IActionResult PutDiaryEntry(int id, DiaryEntry diaryEntry)
    {
        if (id != diaryEntry.Id)
        {
            return BadRequest();
        }

        _dbContext.Entry(diaryEntry).State = EntityState.Modified;

        try
        {
            _dbContext.SaveChanges();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!DiaryEntryExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // DELETE: api/DiaryEntries/5
    [HttpDelete("{id}")]
    public IActionResult DeleteDiaryEntry(int id)
    {
        var diaryEntry = _dbContext.DiaryEntries.Find(id);
        if (diaryEntry == null)
        {
            return NotFound();
        }

        _dbContext.DiaryEntries.Remove(diaryEntry);
        _dbContext.SaveChanges();

        return NoContent();
    }
    private bool DiaryEntryExists(int id)
    {
        return _dbContext.DiaryEntries.Any(e => e.Id == id);
    }
}
