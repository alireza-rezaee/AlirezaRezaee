﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Rezaee.Alireza.Web.Data;
using Rezaee.Alireza.Web.Helpers;
using Rezaee.Alireza.Web.Models.ViewModels.Pages;
using Rezaee.Alireza.Web.Extensions;
using System.Runtime.InteropServices.ComTypes;
using Microsoft.AspNetCore.Authorization;

namespace Rezaee.Alireza.Web.Controllers
{
    [Route("pages")]
    public class PagesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileManager _ifileManager;
        private readonly IWebHostEnvironment _env;

        public PagesController(ApplicationDbContext context, IFileManager ifileManager, IWebHostEnvironment env)
        {
            _context = context;
            _ifileManager = ifileManager;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Pages.Select(page => new IndexViewModel
            {
                Id = page.Id,
                Title = page.Title,
                Path = page.Path,
                Summary = page.Html.Length >= 500 ? page.Html.Substring(0, 500) : page.Html.Substring(0, page.Html.Length),
                ImageCoverPath = page.ImageCoverPath,
                CreateDateTime = page.CreateDateTime,
                UpdateDateTime = page.UpdateDateTime
            }).ToListAsync());
        }

        [Route("/{path:pagePath}")]
        public async Task<IActionResult> Detail(string path)
        {
            if (string.IsNullOrEmpty(path))
                return NotFound();

            var page = await _context.Pages.Where(page => page.Path == path).FirstOrDefaultAsync();
            if (page == null) return NotFound();

            ViewData["Title"] = page.Title;
            return View(page);
        }

        [Route("create")]
        [Authorize(Roles = Roles.PageCreate)]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Roles.PageCreate)]
        public async Task<IActionResult> Creating(CreateEditViewModel createVM)
        {
            if (ModelState.IsValid)
            {
                var persianDateTime = PersianDateTime.Now;
                var dateTime = persianDateTime.ToDateTime();
                var newPage = createVM.Page;
                var newImage = createVM.CoverImage;
                try
                {
                    newPage.Path = Helpers.File.ValidateName(newPage.Path);

                    if (newPage.HasLayout == true && newImage != null)
                        throw new Exception("در صورت انتخاب «مستقل از پوسته سایت» نباید از این طریق تصویری انتخاب شود.");
                    await CheckTitleExistence(newPage.Title);

                    if (newImage != null)
                    {
                        newImage.Check(1048576, new string[] { "image/jpg", "image/jpeg", "image/png", "image/gif" });
                        var newImagePath = "uploads/images/"
                            + persianDateTime.ToString("yyyy/MM/dd/yyyyMMddhhmmss")
                            + DateTime.Now.ToString("ffff") + new Random().Next(1000000, 9999999) + "_" + Helpers.File.ValidateName(newImage.FileName);
                        await _ifileManager.SaveFile(newImage, newImagePath);
                        newPage.ImageCoverPath = "/" + newImagePath;
                    }

                    newPage.CreateDateTime = dateTime;

                    _context.Add(newPage);
                    await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

            return View(nameof(Create), createVM);
        }

        [Route("{id}/{path}/edit")]
        [Authorize(Roles = Roles.PageEdit)]
        public async Task<IActionResult> Edit(int id, string path)
        {
            var page = await _context.Pages.FirstOrDefaultAsync(page => page.Id == id && page.Path == path);
            if (page == null) return NotFound();

            ViewData["Title"] = $"اصلاح صفحه «{page.Title}»";
            return View(page);
        }

        [HttpPost("{id}/edit")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Roles.PageEdit)]
        public async Task<IActionResult> Editing(CreateEditViewModel editVM, int id)
        {
            var page = await _context.Pages.FirstOrDefaultAsync(page => page.Id == id);
            if (page == null) return NotFound();

            if (ModelState.IsValid)
            {
                var persianDateTime = PersianDateTime.Now;
                var dateTime = persianDateTime.ToDateTime();
                var newPage = editVM.Page;
                var newImage = editVM.CoverImage;

                try
                {
                    newPage.Path = Helpers.File.ValidateName(newPage.Path);

                    if (newPage.HasLayout == true && newImage != null)
                        throw new Exception("در صورت انتخاب «مستقل از پوسته سایت» نباید از این طریق تصویری انتخاب شود.");
                    await CheckTitleExistence(newPage.Title, id);

                    if (newImage != null)
                    {
                        newImage.Check(1048576, new string[] { "image/jpg", "image/jpeg", "image/png", "image/gif" });
                        var newImagePath = "uploads/images/"
                            + persianDateTime.ToString("yyyy/MM/dd/yyyyMMddhhmmss")
                            + DateTime.Now.ToString("ffff") + new Random().Next(1000000, 9999999) + "_" + Helpers.File.ValidateName(newImage.FileName);
                        await _ifileManager.SaveFile(newImage, newImagePath);
                        newPage.ImageCoverPath = "/" + newImagePath;
                    }

                    newPage.CreateDateTime = dateTime;

                    _context.Update(newPage);
                    await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

            return View(nameof(Edit), "Pages");
        }

        [HttpPost("{id}/{path}/delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Roles.PageDelete)]
        public async Task<IActionResult> Deleting(int id, string path)
        {
            try
            {
                var page = await _context.Pages.FirstOrDefaultAsync(page => page.Id == id && page.Path == path);
                if (page == null) return NotFound();

                _ifileManager.DeleteFile(page.ImageCoverPath);

                _context.Remove(page);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index), "Pages");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private async Task CheckTitleExistence(string title)
        {
            if (await _context.Pages.AsNoTracking().AnyAsync(page => page.Title == title.Trim()))
                throw new Exception("لطفاً نشانی دیگری برای این صفحه انتخاب کنید؛ این نشانی تکراری است.");
        }

        private async Task CheckTitleExistence(string title, int skipPageId)
        {
            if (!await _context.Pages.AsNoTracking().Where(page => page.Id != skipPageId).AnyAsync(page => page.Title == title.Trim()))
                throw new Exception("لطفاً نشانی دیگری برای این صفحه انتخاب کنید؛ این نشانی تکراری است.");
        }
    }
}
