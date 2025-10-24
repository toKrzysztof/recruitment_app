import { JwtPayload } from 'jwt-decode';
import { EMAILCLAIMJWTTOKENKEY } from '../../auth/constants/jwt-constants';

export interface JwtDecoded extends JwtPayload {
  [EMAILCLAIMJWTTOKENKEY]?: string;
}
