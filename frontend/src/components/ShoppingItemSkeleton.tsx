import Skeleton from "./Skeleton";

export default function ShoppingItemSkeleton() {
  return (
    <div className="flex w-full items-center gap-3 rounded-xl border border-gray-200 bg-white px-4 py-3">
      <Skeleton className="h-5 w-5 shrink-0 rounded-full" />
      <Skeleton className="h-4 flex-1" />
      <Skeleton className="h-3 w-10" />
    </div>
  );
}
