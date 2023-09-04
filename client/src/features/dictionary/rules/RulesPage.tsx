import { Link, useNavigate } from 'react-router-dom';
import { Button, Flag, Icon, Table } from 'semantic-ui-react';

import EmptyBlock from '../../../components/EmptyBlock';
import StatusLabel from '../StatusLabel';

export default function RulesPage() {
  const navigate = useNavigate();

  return (
    <>
      <div className="text-end mb-3">
        <Button
          as={Link} to="/dictionary/rules/add"
        >
          <Icon name="add" />
          Add rule
        </Button>
      </div>
      {false
        ? <EmptyBlock />
        : <div className="table-responsive">
          <Table selectable unstackable>
            <Table.Body>
              {Array(15).fill(null).map((_, index) => (
                <Table.Row
                  key={index}
                  className="cursor-pointer"
                  verticalAlign="top"
                  onClick={() => navigate(`/dictionary/rules/${index}`)}
                >
                  <Table.Cell collapsing>
                    <Icon name="block layout" />
                  </Table.Cell>
                  <Table.Cell collapsing>
                    <Flag name="america" />
                  </Table.Cell>
                  <Table.Cell collapsing>
                    <StatusLabel status={'Status'} />
                  </Table.Cell>
                  <Table.Cell>
                    Rule
                  </Table.Cell>
                </Table.Row>
              ))}
            </Table.Body>
          </Table>
        </div>
      }
    </>
  );
}