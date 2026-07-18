// CLI: download VRT baselines from the cloud storage.
// Run with `npm run snapshots:pull [-- <options>]`; see --help for options.
import { runSyncCli } from "./snapshot-sync.ts";
import { storageAdapter } from "./snapshot-storage.ts";

await runSyncCli("pull", storageAdapter);
