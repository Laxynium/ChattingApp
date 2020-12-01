import {FormControl, ValidationErrors, ValidatorFn} from '@angular/forms';

export function requiredFileType(type: string): ValidatorFn {
  return (control: FormControl): ValidationErrors | null => {
    const file = control.value;
    if (file) {
      const extension = file.split('.')[1].toLowerCase();
      if (type.toLowerCase() !== extension.toLowerCase()) {
        return {
          requiredFileType: true,
        };
      }

      return null;
    }

    return null;
  };
}
