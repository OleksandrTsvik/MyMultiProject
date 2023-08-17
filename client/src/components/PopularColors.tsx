interface Props {
  onClickColor: (color: string) => void;
}

export const popularColors = [
  '#f5222d', '#fa541c', '#fa8c16',
  '#faad14', '#fadb14', '#a0d911',
  '#52c41a', '#13c2c2', '#1677ff',
  '#2f54eb', '#722ed1', '#eb2f96',
  '#ffffff', '#222222', '#000000'
];

export default function PopularColors({ onClickColor }: Props) {
  return (
    <div className="d-flex flex-wrap gap-2 pt-1 pb-2">
      {popularColors.map((color, index) => (
        <div
          key={index}
          style={{ backgroundColor: color }}
          className="duty__pick-color"
          onClick={() => onClickColor(color)}
        />
      ))}
    </div>
  );
}