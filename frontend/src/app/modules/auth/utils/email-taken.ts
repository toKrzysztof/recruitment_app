import { emailTakenMessage } from '../constants/error-messages';

export const errorsIncludeEmailTaken = (response: unknown) => {
  if (response && typeof response === 'object' && 'error' in response) {
    const errors = response.error;

    if (Array.isArray(errors)) {
      return errors.some((error: unknown) => error === emailTakenMessage);
    }
  }

  return false;
};
