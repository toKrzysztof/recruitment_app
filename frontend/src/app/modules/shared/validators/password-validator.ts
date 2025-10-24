import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export function passwordComplexityValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const value = control.value;
    const errors: ValidationErrors = {};

    if (value == null || value === '') {
      errors['required'] = true;
    }

    const lengthValid = value.length >= 8;
    const hasNumber = /[0-9]/.test(value);
    const hasNonAlphanumeric = /[^a-zA-Z0-9]/.test(value);

    if (!lengthValid) {
      errors['minLength'] = { requiredLength: 8, actualLength: value.length };
    }
    if (!hasNumber) {
      errors['numberRequired'] = true;
    }
    if (!hasNonAlphanumeric) {
      errors['specialCharRequired'] = true;
    }

    return Object.keys(errors).length > 0 ? errors : null;
  };
}
