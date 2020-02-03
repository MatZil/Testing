import { Newclient } from "../../models/newclient";

export interface ClientModal {
  clientFormData: Newclient,
  formTitle: string,
  formConfirmationButtonName: string
}
