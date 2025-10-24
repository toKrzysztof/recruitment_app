export interface SubcategoryDto {
  readonly id?: number;
  readonly name: string;
  readonly subcategory: SubcategoryDto;
}
