using System;
using System.Web.Mvc;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace Sitecore.UxExtensions.GotoReference.Controllers
{
    public class FieldDataController : Controller
    {
        public JsonResult GetReferencedItemData(Guid itemId, Guid fieldId)
        {
            Item item = Sitecore.Context.Database.GetItem(new ID(itemId));

            if (item == null)
            {
                return Json(string.Empty, JsonRequestBehavior.AllowGet);
            }

            Field field = item.Fields[new ID(fieldId)];

            if (field == null)
            {
                return Json(string.Empty, JsonRequestBehavior.AllowGet);
            }

            if (field.TypeKey == "image")
            {
                ImageField imageField = field;
                if (imageField != null && imageField.MediaID != ID.Null)
                {
                    return Json(imageField.MediaID.Guid.ToString("B").ToUpper(), JsonRequestBehavior.AllowGet);
                }
            }
            else if (field.TypeKey == "droplist" && !string.IsNullOrEmpty(field.Value))
            {
                Item target = item.Database.GetItem(StringUtil.Combine(field.Source, field.Value, "/"));
                if (target != null)
                {
                    return Json(target.ID.Guid.ToString("B").ToUpper(), JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ReferenceField referenceField = field;
                if (referenceField != null && referenceField.TargetID != ID.Null)
                {
                    return Json(referenceField.TargetID.Guid.ToString("B").ToUpper(), JsonRequestBehavior.AllowGet);
                }
            }

            return Json(string.Empty, JsonRequestBehavior.AllowGet);
        }
    }
}
