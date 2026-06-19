import Skeleton from "./Skeleton";

export default function ListCardSkeleton() {
  return (
    <div className="flex w-full items-center justify-between rounded-xl border border-gray-200 bg-white px-4 py-3">
      <div className="flex-1 space-y-2">
        <Skeleton className="h-4 w-2/3" />
        <Skeleton className="h-3 w-1/3" />
      </div>
    </div>
  );
}
