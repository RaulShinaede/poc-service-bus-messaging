namespace EventReceiverService.Services {
    public static class HelloCounter {

        private static int count = 0;
        public static void Increment() => count++;
        public static int GetCount() => count;
    }
}
