import { ReactNode } from 'react';
import { makeAutoObservable } from 'mobx';
import { StrictModalProps } from 'semantic-ui-react';

export default class ModalStore {
  isOpen: boolean = false;
  body: ReactNode | null = null;
  props: StrictModalProps = {};
  replaceContent: boolean = false;

  constructor() {
    makeAutoObservable(this);
  }

  openModal = (
    content: ReactNode,
    props: StrictModalProps = {},
    replaceContent: boolean = false
  ) => {
    this.isOpen = true;
    this.body = content;
    this.props = props;
    this.replaceContent = replaceContent;
  }

  closeModal = () => {
    this.isOpen = false;
    this.body = null;
    this.props = {};
    this.replaceContent = false;
  }
}