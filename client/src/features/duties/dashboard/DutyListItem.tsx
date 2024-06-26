import { DragEvent, MouseEvent, useEffect, useState } from 'react';
import { observer } from 'mobx-react-lite';
import { Button, Card, Grid, Icon, Popup } from 'semantic-ui-react';
import { Form, Formik } from 'formik';
import { format } from 'date-fns';

import { Duty } from '../../../app/models/duty';
import { useStore } from '../../../app/stores/store';
import { Style } from '../modals/DutyModalCreate';
import FormikDutyPickColor from './FormikDutyPickColor';

interface Props {
  duty: Duty;
  draggable?: boolean;
  onMouseDownDraggableElem: (event: MouseEvent<HTMLDivElement>) => void;
  onMouseUpDraggableElem: (event: MouseEvent<HTMLDivElement>) => void;
  onDragStart: (event: DragEvent<HTMLDivElement>, duty: Duty) => void;
  onDragEnd: (event: DragEvent<HTMLDivElement>) => void;
  onDragOver: (event: DragEvent<HTMLDivElement>, duty: Duty) => void;
}

export default observer(function DutyListItem(
  {
    duty,
    draggable,
    onMouseDownDraggableElem,
    onMouseUpDraggableElem,
    onDragStart,
    onDragEnd,
    onDragOver
  }: Props
) {
  const { dutyStore } = useStore();
  const {
    updateIsCompletedDuty, getIsLoading, openEditMode,
    openDeleteMode, toggleChangeColorMode, getIsChangeColor,
    changeColor
  } = dutyStore;

  const [dutyItem, setDutyItem] = useState<Duty>(duty);
  const [style, setStyle] = useState<Style>({
    backgroundColor: dutyItem.backgroundColor,
    color: dutyItem.fontColor,
    borderColor: dutyItem.fontColor
  });

  useEffect(() => {
    setDutyItem(duty);
  }, [duty]);

  const classesForContainer = 'duty__item rounded'
    + (getIsChangeColor(dutyItem.id) ? ' duty__collapsible' : '');

  const dragAndDrop = draggable
    ? {
      className: `${classesForContainer} duty__draggable`,
      onDragStart: (e: DragEvent<HTMLDivElement>) => onDragStart(e, dutyItem),
      onDragEnd: onDragEnd,
      onDragOver: (e: DragEvent<HTMLDivElement>) => onDragOver(e, dutyItem)
    }
    : {
      className: classesForContainer
    };

  return (
    <Grid.Column
      mobile={16}
      tablet={8}
      computer={4}
      {...dragAndDrop}
    >
      <Card fluid style={style}>
        <Card.Content>
          {!dutyItem.isCompleted && draggable
            ? <Card.Header
              style={style}
              className="d-flex justify-content-between"
            >
              <div>{dutyItem.title}</div>
              <div
                className="draggable"
                onMouseDown={onMouseDownDraggableElem}
                onMouseUp={onMouseUpDraggableElem}
              >
                <Icon name="grid layout" />
              </div>
            </Card.Header>
            : <Card.Header style={style}>{dutyItem.title}</Card.Header>
          }
        </Card.Content>
        <Card.Content style={style}>
          <Card.Description
            style={style}
            className="formatted-text"
          >
            {dutyItem.description}
          </Card.Description>
        </Card.Content>
        <Card.Content style={style}>
          <Card.Meta style={style}>
            <Popup
              content="date create"
              position="bottom left"
              trigger={
                <div>
                  <Icon name="calendar alternate" />&ensp;
                  {format(dutyItem.dateCreation, "LLLL dd, iiii 'at' kk:mm:ss, y")}
                </div>
              }
            />
          </Card.Meta>
        </Card.Content>
        <Card.Content
          style={style}
          className="d-flex flex-row-reverse gap-2"
        >
          <Button circular
            icon="tint"
            color="purple"
            className="m-0"
            onClick={() => toggleChangeColorMode(dutyItem)}
          />
          <Button inverted circular
            icon="trash alternate"
            color="red"
            className="m-0"
            onClick={() => openDeleteMode(dutyItem.id)}
          />
          <Button inverted circular
            icon="pencil alternate"
            color="blue"
            className="m-0"
            onClick={() => openEditMode(dutyItem.id)}
          />
          {dutyItem.isCompleted
            ? <Button inverted circular
              icon="cancel"
              color="orange"
              className="m-0"
              loading={getIsLoading(dutyItem.id)}
              disabled={getIsLoading(dutyItem.id)}
              onClick={() => updateIsCompletedDuty(dutyItem, false)}
            />
            : <Button inverted circular
              icon="check"
              color="green"
              className="m-0"
              loading={getIsLoading(dutyItem.id)}
              disabled={getIsLoading(dutyItem.id)}
              onClick={() => updateIsCompletedDuty(dutyItem, true)}
            />
          }
        </Card.Content>
        {getIsChangeColor(dutyItem.id) &&
          <Card.Content
            style={style}
          >
            <Formik initialValues={dutyItem} onSubmit={changeColor}>
              {({ submitForm }) => (
                <Form className="ui form">
                  <FormikDutyPickColor
                    style={style}
                    setStyle={setStyle}
                    onChangeColor={submitForm}
                  />
                </Form>
              )}
            </Formik>
          </Card.Content>
        }
      </Card>
    </Grid.Column>
  );
});