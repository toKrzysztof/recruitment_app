import { FormControl } from '@angular/forms';
import { CategoryDto } from './category-dto';
import { SubcategoryDto } from './subcategory-dto';

export interface ContactForm {
  email: FormControl<string>;
  firstName: FormControl<string>;
  lastName: FormControl<string>;
  password: FormControl<string>;
  phoneNumber: FormControl<string>;
  dateOfBirth: FormControl<Date | null>;
  category: FormControl<CategoryDto | null>;
  subcategory: FormControl<SubcategoryDto | null>;
}
