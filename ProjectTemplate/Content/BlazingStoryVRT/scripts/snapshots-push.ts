// CLI: upload VRT baselines to the cloud storage.
// Run with `npm run snapshots:push [-- <options>]`; see --help for options.
import { runSyncCli } from "./snapshot-sync.ts";
import { storageAdapter } from "./snapshot-storage.ts";

await runSyncCli("push", storageAdapter);
