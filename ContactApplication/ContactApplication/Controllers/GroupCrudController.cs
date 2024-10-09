using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ContactApplication.Data;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ContactApplication.Models;
using ContactApplication.ViewModels;

using Microsoft.EntityFrameworkCore;

namespace ContactBookApp.Controllers
{
   public class GroupCrudController : Controller
   {
       private readonly ApplicationDbContext _context;
       
       private readonly UserManager<User> _userManager;


       public GroupCrudController(ApplicationDbContext context, UserManager<User> userManager)
       {
           _context = context;
           _userManager = userManager;
       }


       // GET: GroupCrud
       public async Task<IActionResult> Index()
       {
           var user = await _userManager.GetUserAsync(User);
           if (user == null)
           {
               return NotFound("User not found.");
           }
           // Pass the username to the layout view
           ViewBag.Username = user.UserName;
// Retrieve contacts where iduser matches the current user's id
           var groups = await _context.Groups
               .Where(g => g.UserId == user.Id)
               .ToListAsync();

           return View(groups);
          
       }


       // GET: GroupCrud/Details/5
       public async Task<IActionResult> Details(int? id)
       {
           ViewBag.GroupId = id;
           if (id == null)
           {
               return NotFound();
           }

           // Retrieve the contacts of the specified group
           var contacts = await _context.ContactGroups
               .Where(cg => cg.GroupId == id)
               .Select(cg => cg.Contact)
               .ToListAsync();

           if (contacts == null)
           {
               return NotFound();
           }

           return View(contacts);
       }


       // GET: GroupCrud/Create
       public IActionResult Create()
       {
           return View();
       }


       // POST: GroupCrud/Create
       // To protect from overposting attacks, enable the specific properties you want to bind to.
       // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
       [HttpPost]
       [ValidateAntiForgeryToken]
       public async Task<IActionResult> Create([Bind("GroupId,GroupName")] Group @group)
       {
           var user = await _userManager.GetUserAsync(User);
           group.UserId = user.Id;
               _context.Groups.AddAsync(@group);
               await _context.SaveChangesAsync();
               return RedirectToAction(nameof(Index));
           
           return View(@group);
       }


       // GET: GroupCrud/Edit/5
       public async Task<IActionResult> Edit(int? id)
       {
           var user = await _userManager.GetUserAsync(User);
           if (id == null)
           {
               return NotFound();
           }


           var @group = await _context.Groups.FindAsync(id);
           if (@group == null)
           {
               return NotFound();
           }
           return View(@group);
       }


       // POST: GroupCrud/Edit/5
       // To protect from overposting attacks, enable the specific properties you want to bind to.
       // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
       [HttpPost]
       [ValidateAntiForgeryToken]
       public async Task<IActionResult> Edit(int id, [Bind("GroupId,GroupName")] Group @group)
       {
           var user = await _userManager.GetUserAsync(User);
           if (id != @group.GroupId)
           {
               return NotFound();
           }


           
               try
               {
                   
                   group.UserId = user.Id;
                   _context.Update(@group);
                   await _context.SaveChangesAsync();
               }
               catch (DbUpdateConcurrencyException)
               {
                   if (!GroupExists(@group.GroupId))
                   {
                       return NotFound();
                   }
                   else
                   {
                       throw;
                   }
               }
               return RedirectToAction(nameof(Index));
           
           return View(@group);
       }


       // GET: GroupCrud/Delete/5
       public async Task<IActionResult> Delete(int? id)
       {
           if (id == null)
           {
               return NotFound();
           }


           var @group = await _context.Groups
               .FirstOrDefaultAsync(m => m.GroupId == id);
           if (@group == null)
           {
               return NotFound();
           }


           return View(@group);
       }


       // POST: GroupCrud/Delete/5
       [HttpPost, ActionName("Delete")]
       [ValidateAntiForgeryToken]
       public async Task<IActionResult> DeleteConfirmed(int id)
       {
           var @group = await _context.Groups.FindAsync(id);
           if (@group != null)
           {
               _context.Groups.Remove(@group);
           }


           await _context.SaveChangesAsync();
           return RedirectToAction(nameof(Index));
       }
     
    
       
       public async Task< IActionResult> AddContactGroupExist(int id)
       {
           
           
           ViewBag.GroupId = id;
           var user = await _userManager.GetUserAsync(User);
           if (user == null)
           {
               return NotFound("User not found.");
           }
           // Pass the username to the layout view
           ViewBag.Username = user.UserName;

           // Retrieve contacts where iduser matches the current user's id
           var contacts = await _context.Contacts
               .Where(c => c.UserId == user.Id) // Filter contacts by user
               .Where(c => !_context.ContactGroups.Any(cg => cg.ContactId == c.ContactId && cg.GroupId == id)) // Filter out contacts that exist in ContactGroup for the specified group
               .ToListAsync();
           return View(contacts);
           
       }

       [HttpPost]
       public async Task<IActionResult> AddContactGroupExist(int contactId, int groupId)
       {
           Console.WriteLine("ContactId:", contactId);
           var user = await _userManager.GetUserAsync(User);
           ViewBag.GroupId = groupId;
           ViewBag.Username = user.UserName;
           if (user != null)
           {
               if (user.Id != null)
               {
                   if (contactId != null)
                   {
                       if (groupId != null)
                       {
                           var contactGroup = new ContactGroup
                           {
                               GroupId = groupId, 
                               ContactId = contactId 
                           };

                           // Add the ContactGroup to the database
                           await _context.ContactGroups.AddAsync(contactGroup);
                           await _context.SaveChangesAsync();
                           return RedirectToAction(nameof(Index));
                       }
                   }
               }
           } else
           {
               return NotFound("Userid not found.");
           }

           return View();
       
       }

       [HttpPost]
       public async Task<IActionResult> removeContact(int contactId, int groupId)
       {
           ViewBag.GroupId = groupId;
           Console.WriteLine("ContactId:", contactId);
           Console.WriteLine("groupId:", groupId);
           
           // Find the ContactGroup entry with the specified contactId and groupId
           var contactGroup = await _context.ContactGroups
               .FirstOrDefaultAsync(cg => cg.ContactId == contactId && cg.GroupId == groupId);

           // If the entry exists, remove it from the context and save changes
           if (contactGroup != null)
           {
               _context.ContactGroups.Remove(contactGroup);
               await _context.SaveChangesAsync();
           }

           // Redirect back to the group details page
           return RedirectToAction(nameof(Details), new { id = groupId });
       }
       
       public IActionResult AddContactGroupNew(int id)
       {
           return View();
       }
    
       [HttpPost]
       [ValidateAntiForgeryToken]
       public async Task<IActionResult> AddContactGroupNew(int id,AddContactVM addContactVm)
       {
           ViewBag.GroupId = id;
           
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
                   // Create a new ContactGroup
                   var contactGroup = new ContactGroup
                   {
                       GroupId = id, // Assuming 'id' is the ID of the group
                       ContactId = contact.ContactId // Get the ID of the newly created contact
                   };

                   // Add the ContactGroup to the database
                   await _context.ContactGroups.AddAsync(contactGroup);
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

       
    
       private bool GroupExists(int id)
       {
           return _context.Groups.Any(e => e.GroupId == id);
       }
   }
}
