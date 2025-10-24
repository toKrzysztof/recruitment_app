import { CategoryDto } from './category-dto';
import { SubcategoryDto } from './subcategory-dto';

export interface ContactDetailsDto {
  readonly id: string;
  readonly firstName: string;
  readonly lastName: string;
  readonly email: string;
  readonly phoneNumber: string;
  readonly password: string;
  readonly dateOfBirth: string;
  readonly category: CategoryDto;
  readonly subcategory: SubcategoryDto;
}
