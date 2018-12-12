import { Pagination } from "./Pagination";
import { GenderEnum } from "./MemberProfile";

        
//Must be same with the TermEnum on Property.cs
export enum TermEnum {
    SHORT,
    WEEK,
    WEEK_2,
    WEEK_3,
    MONTH,
    MONTH_2,
    MONTH_3,
    LONG
}

export enum PropertyTypeEnum {
    HOUSE,
    APPARTMENT,
    UNIT
}

export enum RentTypeEnum {
    OWN,
    SHARE,
    WHOLE,
    HOMESTAY
}

export enum PropertyRequestEnum {
    NotCare,
    Yes,
    No
}

export interface PropertySearchOptions {
    appUserId?: number;
    categoryIds: number[];
    cityId: number;
    gender?: GenderEnum;
    rentTypes: RentTypeEnum[];
    pet: PropertyRequestEnum; //0: not care 1: want 2: not want
    child: PropertyRequestEnum;
    internet: number; //0:none 1: limited 2:unlimited
    area_top?: number;
    area_left?: number;
    area_right?: number;
    area_bottom?: number;
    dateFrom?: Date;
    mateMax?: number;
    mateMin?: number;
    rentMin?: number;
    rentMax?: number;
    minTerm?: TermEnum;
    maxTerm?: TermEnum;
    paginationParams: Pagination;
}

// public int[] CategoryIds {get; set;}
//         public int CityId {get; set;}
//         public PropertyRequestEnum Pet {get; set;} = PropertyRequestEnum.NotCare;
//         public PropertyRequestEnum Chiled {get; set;} = PropertyRequestEnum.NotCare;
//         public int Internet { get;set;} = 0; //0: not care, 1: limited 3:unlimited
//         public double? Area_top{ get; set;} = null;
//         public double? Area_left{get; set;} = null;
//         public double? Area_right{get; set;} = null;
//         public double? Area_bottom{get; set;} = null;
//         public DateTime? DateFrom{get; set;} = null;
//         public int? MateMax{get; set;} = null;
//         public int? MateMin{get; set;} = null;
//         public int? rentMin{get; set;} = null;
//         public int? rentMax{get; set;} = null;
//         public TermEnum minTerm{get; set;} = TermEnum.SHORT;
//         public TermEnum maxTerm{get; set;} = TermEnum.LONG;
