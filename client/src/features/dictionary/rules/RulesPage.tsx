import { useEffect } from 'react';
import { observer } from 'mobx-react-lite';
import { Link, useNavigate } from 'react-router-dom';
import { Button, Icon, Table } from 'semantic-ui-react';

import { useStore } from '../../../app/stores/store';
import EmptyBlock from '../../../components/EmptyBlock';
import Loading from '../../../components/Loading';
import CustomFlag from '../../../components/CustomFlag';
import StatusLabel from '../StatusLabel';

export default observer(function RulesPage() {
  const navigate = useNavigate();
  const { dictionaryStore } = useStore();

  const {
    loadGrammarRules, loadingGrammarRules,
    grammarRulesSortByPosition, resetGrammarRules
  } = dictionaryStore;

  useEffect(() => {
    loadGrammarRules();

    return () => {
      resetGrammarRules();
    }
  }, [loadGrammarRules, resetGrammarRules]);

  if (loadingGrammarRules) {
    return <Loading content="Loading grammar rules..." />;
  }

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
      {grammarRulesSortByPosition.length === 0
        ? <EmptyBlock />
        : <div className="table-responsive">
          <Table selectable unstackable>
            <Table.Body>
              {grammarRulesSortByPosition.map((rule) => (
                <Table.Row
                  key={rule.id}
                  className="cursor-pointer"
                  verticalAlign="top"
                  onClick={() => navigate(`/dictionary/rules/${rule.id}`)}
                >
                  <Table.Cell collapsing>
                    <Icon name="block layout" />
                  </Table.Cell>
                  <Table.Cell collapsing>
                    <CustomFlag strName={rule.language} />
                  </Table.Cell>
                  <Table.Cell collapsing>
                    <StatusLabel className="m-0" status={rule.status} />
                  </Table.Cell>
                  <Table.Cell>
                    {rule.title}
                  </Table.Cell>
                </Table.Row>
              ))}
            </Table.Body>
          </Table>
        </div>
      }
    </>
  );
});