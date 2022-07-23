using SuitAlterationManager.Domain.AlterationManagement.Enum;
using SuitAlterationManager.Domain.AlterationManagement.ValueObjects;
using SuitAlterationManager.Domain.Base.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SuitAlterationManager.Domain.AlterationManagement
{
    //public static class AlterationTypes
    //{
    //    public static readonly AlterationType LeftSleevesMinus5 = new AlterationType(code: "LeftSleevesMinus5",
    //                                                                                 description: "Left Sleeves, -5 cm",
    //                                                                                 size: -5,
    //                                                                                 direction: AlterationTypeDirection.Left);
    //    public static readonly AlterationType LeftSleevesPlus5 = new AlterationType(code: "LeftSleevesPlus5",
    //                                                                                 description: "Left Sleeves, 5 cm",
    //                                                                                 size: 5,
    //                                                                                 direction: AlterationTypeDirection.Left);
    //    public static readonly AlterationType RightSleevesMinus5 = new AlterationType(code: "RightSleevesMinus5",
    //                                                                                 description: "Right Sleeves, -5 cm",
    //                                                                                 size: -5,
    //                                                                                 direction: AlterationTypeDirection.Right);
    //    public static readonly AlterationType RightSleevesPlus5 = new AlterationType(code: "RightSleevesPlus5",
    //                                                                                 description: "Right Sleeves, 5 cm",
    //                                                                                 size: 5,
    //                                                                                 direction: AlterationTypeDirection.Right);
    //    public static readonly AlterationType LeftRightSleevesPlus5 = new AlterationType(code: "LeftRightSleevesPlus5",
    //                                                                                 description: "Left and Right Sleeves, 5 cm",
    //                                                                                 size: 5,
    //                                                                                 direction: AlterationTypeDirection.Left);
    //    public static readonly AlterationType LeftRightSleevesMinus5 = new AlterationType(code: "LeftRightSleevesPlus5",
    //                                                                                 description: "Left and Right Sleeves, 5 cm",
    //                                                                                 size: -5,
    //                                                                                 direction: AlterationTypeDirection.Left);
    //    public static readonly AlterationType LeftTrousersMinus5 = new AlterationType(code: "LeftTrousersMinus5",
    //                                                                                 description: "Left Trousers, -5 cm",
    //                                                                                 size: -5,
    //                                                                                 direction: AlterationTypeDirection.Left);
    //    public static readonly AlterationType LeftTrousersPlus5 = new AlterationType(code: "LeftTrousersPlus5",
    //                                                                                 description: "Left Trousers, 5 cm",
    //                                                                                 size: 5,
    //                                                                                 direction: AlterationTypeDirection.Left);
    //    public static readonly AlterationType RightTrousersMinus5 = new AlterationType(code: "RightTrousersMinus5",
    //                                                                                 description: "Right Trousers, -5 cm",
    //                                                                                 size: -5,
    //                                                                                 direction: AlterationTypeDirection.Right);
    //    public static readonly AlterationType RightTrousersPlus5 = new AlterationType(code: "RightTrousersPlus5",
    //                                                                                 description: "Right Trousers, 5 cm",
    //                                                                                 size: 5,
    //                                                                                 direction: AlterationTypeDirection.Right);
    //    public static readonly AlterationType LeftRightTrousersPlus5 = new AlterationType(code: "LeftRightTrousersPlus5",
    //                                                                                 description: "Left and Right Trousers, 5 cm",
    //                                                                                 size: 5,
    //                                                                                 direction: AlterationTypeDirection.Left);
    //    public static readonly AlterationType LeftRightTrousersMinus5 = new AlterationType(code: "LeftRightTrousersPlus5",
    //                                                                                 description: "Left and Right Trousers, 5 cm",
    //                                                                                 size: -5,
    //                                                                                 direction: AlterationTypeDirection.Left);

    //    public static readonly List<AlterationType> AllAlterationTypes = new List<AlterationType>() { LeftSleevesMinus5, LeftSleevesPlus5, RightSleevesMinus5 , RightSleevesPlus5 ,
    //                                                                                                  LeftRightSleevesPlus5, LeftRightSleevesMinus5,LeftTrousersMinus5, LeftTrousersPlus5,
    //                                                                                                  RightTrousersMinus5, RightTrousersPlus5, LeftRightTrousersPlus5, LeftRightTrousersMinus5 };
    //    public static AlterationType GetAlterationTypeByCode(string code)
    //    {
    //        var type = AllAlterationTypes.SingleOrDefault(x => x.Code == code);
    //        if (type == null)
    //            throw new ArgumentNullException("Unable to find an alteration type with code " + code);
    //        return AllAlterationTypes.SingleOrDefault(x => x.Code == code);
    //    }
    //    public class AlterationType
    //    {
    //        public AlterationType(string code, string description, int size, AlterationTypeDirection direction)
    //        {
    //            this.Code = code;
    //            this.Description = description;
    //            this.Size = size;
    //            this.Direction = direction;
    //        }
    //        public string Code { get; set; }
    //        public string Description { get; set; }
    //        public int Size { get; set; }
    //        public AlterationTypeDirection Direction { get; set; }
    //    }
    //}
}
