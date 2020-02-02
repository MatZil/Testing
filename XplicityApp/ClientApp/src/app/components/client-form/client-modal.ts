import { ModalType } from "../../enums/modal-type.enum";
import { Newclient } from "../../models/newclient";

export interface ClientModal {
  clientFormData: Newclient,
  modalType: ModalType
}
