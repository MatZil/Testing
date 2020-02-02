import { Newclient } from "../../models/newclient";
import { ModalType } from "./modal-type.enum";

export interface ClientModal {
  clientFormData: Newclient,
  modalType: ModalType
}
