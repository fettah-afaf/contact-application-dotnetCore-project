using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ContactApplication.Data;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ContactApplication.Models;
using ContactApplication.ViewModels;

using Microsoft.EntityFrameworkCore;
namespace ContactApplication.Controllers;

public class ContactController :Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<User> _userManager;

    public ContactController(ApplicationDbContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }
    public async Task<IActionResult> Index()
    {
        // Get the current user
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound("User not found.");
        }
        // Pass the username to the layout view
        ViewBag.Username = user.UserName;

        // Retrieve contacts where iduser matches the current user's id
        var contacts = await _context.Contacts
            .Where(c => c.UserId == user.Id)
            .ToListAsync();

        return View(contacts);
    }
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }


        var contact = await _context.Contacts
            .FirstOrDefaultAsync(m => m.ContactId == id);
        if (contact == null)
        {
            return NotFound();
        }


        return View(contact);
    }


   
    public IActionResult Create()
    {
        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AddContactVM addContactVm)
    {
        
            
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                if (user.Id != null)
                {
                    var contact = new Contact
                    {
                        Email = addContactVm.Email,
                        Address = addContactVm.Address,
                        FirstName = addContactVm.FirstName,
                        LastName = addContactVm.LastName,
                        Phone = addContactVm.Phone,
                        Notes = addContactVm.Notes,
                        UserId = user.Id,
                    };
                    ViewBag.Username = user.UserName;

                    await _context.Contacts.AddAsync(contact);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return NotFound("Userid not found.");
                }
            }

            return View();
    }
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var contact = await _context.Contacts.FindAsync(id);
        if (contact != null)
        {
            _context.Contacts.Remove(contact);
        }


        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }


        var contact = await _context.Contacts
            .FirstOrDefaultAsync(m => m.ContactId == id);
        if (contact == null)
        {
            return NotFound();
        }


        return View(contact);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("ContactId,FirstName,LastName,Phone,Address,Email,Notes,UserId")] Contact contact)
    {
        if (id != contact.ContactId)
        {
            return NotFound();
        }
        try {
                var user = await _userManager.GetUserAsync(User);
                contact.UserId = user.Id;
                
                _context.Update(contact);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactExists(contact.ContactId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        
        return View(contact);
    }
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }


        var contact = await _context.Contacts.FindAsync(id);
        if (contact == null)
        {
            return NotFound();
        }
        return View(contact);
    }

    private bool ContactExists(int id)
    {
        return _context.Contacts.Any(e => e.ContactId == id);
    }




    


}